using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using My_Profile.Services;
using RabbitMQ.Client;

namespace My_Profile.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private QueueBuilder queue;
        private readonly IUserRepository _userRepository;
        public ValuesController(IUserRepository userRepository, QueueBuilder queue)
        {
            _userRepository = userRepository;
            this.queue = queue;
        }

        // GET api/values
        //Get all users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userRepository.GetAllUsers();
            if (user == null)
            {
                return NotFound("Null database");
            }
            else
            {

                return Ok(user);
            }

        }
        // GET: api/values/id
        //Get user by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            // ObjectId id = new ObjectId(_id);
            var user = await _userRepository.GetUser(id);
            if (user == null)
                return NotFound("user with this id not found");
            return Ok(user);
        }

        [HttpGet("QuizResult/{id}")]
        public async Task<IActionResult> GetQuizResult(string id)
        {
            // ObjectId id = new ObjectId(_id);
            var user = await _userRepository.GetQuizResult(id);
            if (user == null)
                return NotFound("User has not given any tests in this technology");
            return Ok(user);
        }

        [HttpGet("UserReport/{id}")]
        public IActionResult GetUserReport(string id)
        {
            var report = queue.GetUserReport(id);
            return Ok(report);
        }

        //Get Resource Status
        [HttpGet("status/{id}/resource/{resourceId}")]
        public async Task<IActionResult> GetStatusById(string id, string resourceId)
        {
            //  ObjectId id = new ObjectId(_id);
            var status = await _userRepository.GetStatus(id, resourceId);
            return Ok(status);
        }
        // POST: api/values
        //post user
        [HttpPost("{UserNode}")]
        public void PostUserAsync([FromBody] UserWrapper userWrapper)
        {
            var command = new UserWrapper
            {
                UserId = userWrapper.UserId,
            };

            var body = ObjectSerialize.Serialize(command);

            queue.Model.BasicPublish(
                exchange: queue.ExchangeName,
                routingKey: "Users",
                basicProperties: null,
                body: body
            );
            Console.WriteLine($" [x] Sent Post Request for {userWrapper.UserId}");
        }

        [HttpPost("RatingLearningPlan")]
        public void RatingLearningPlanAsync([FromBody] LearningPlanRatingWrapper learningPlanRatingWrapper)
        {
            var command = new LearningPlanRatingWrapper
            {
                // LearningPlanFeedBackId = learningPlanFeedback.LearningPlanFeedBackId,
                LearningPlanId = learningPlanRatingWrapper.LearningPlanId,
                UserId = learningPlanRatingWrapper.UserId,
                Star = learningPlanRatingWrapper.Star,
                // Password = "examplePassword"
            };

            var body = ObjectSerialize.Serialize(command);

            queue.Model.BasicPublish(
                exchange: queue.ExchangeName,
                routingKey: "Send.LearningPlanRating",
                basicProperties: null,
                body: body
            );
            Console.WriteLine($" [x] Sent Rating Request for {learningPlanRatingWrapper.LearningPlanId}");
        }
        

        [HttpPost("RatingResource")]
        public void RatingResourceAsync([FromBody] ResourceFeedBack resourceFeedBack)
        {
            var command = new ResourceFeedBack
            {
                ResourceId = resourceFeedBack.ResourceId,
                UserId = resourceFeedBack.UserId,
                Star = resourceFeedBack.Star,
            };

            var body = ObjectSerialize.Serialize(command);

            queue.Model.BasicPublish(
                exchange: queue.ExchangeName,
                routingKey: "Send.ResourceFeedBack",
                basicProperties: null,
                body: body
            );
            Console.WriteLine($" [x] Sent Rating Request for Resource with id {resourceFeedBack.ResourceId}");

        }

        [HttpPost("SubscriberLearningPlan")]
        public void SubscriberLearningPlanAsync([FromBody] LearningPlanSubscriptionWrapper learningPlanSubscriptionWrapper)
        {
            var command = new LearningPlanSubscriptionWrapper
            {
                LearningPlanId = learningPlanSubscriptionWrapper.LearningPlanId,
                UserId = learningPlanSubscriptionWrapper.UserId,
            };

            var body = ObjectSerialize.Serialize(command);

            queue.Model.BasicPublish(
                exchange: queue.ExchangeName,
                routingKey: "Send.LearningPlanSubscription",
                basicProperties: null,
                body: body
            );
            Console.WriteLine($" [x] {learningPlanSubscriptionWrapper.UserId} Request to Subscribe Learning Plan with Id {learningPlanSubscriptionWrapper.LearningPlanId}");
        }

        [HttpPost("UnSubscriberLearningPlan")]
        public void UnSubscriberLearningPlanAsync([FromBody] LearningPlanSubscriptionWrapper learningPlanSubscriptionWrapper)
        {
            var command = new LearningPlanSubscriptionWrapper
            {
                LearningPlanId = learningPlanSubscriptionWrapper.LearningPlanId,
                UserId = learningPlanSubscriptionWrapper.UserId,
            };

            var body = ObjectSerialize.Serialize(command);

            queue.Model.BasicPublish(
                exchange: queue.ExchangeName,
                routingKey: "Send.LearningPlanSubscription",
                basicProperties: null,
                body: body
            );
            Console.WriteLine($" [x] {learningPlanSubscriptionWrapper.UserId} Request to Unsubscribe Learning Plan with Id {learningPlanSubscriptionWrapper.LearningPlanId}");
        }

        [HttpPost("ReportQuestion")]
        public void ReportQuestionAsync([FromBody] QuestionFeedBack questionFeedBack)
        {
            var command = new QuestionFeedBack
            {
                QuestionFeedBackId = questionFeedBack.QuestionFeedBackId,
                QuestionId = questionFeedBack.QuestionId,
                UserId = questionFeedBack.UserId,
                Ambiguity = questionFeedBack.Ambiguity
            };

            var body = ObjectSerialize.Serialize(command);

            queue.Model.BasicPublish(
                exchange: queue.ExchangeName,
                routingKey: "Send.QuestionFeedBack",
                basicProperties: null,
                body: body
            );
            Console.WriteLine(" [x] Sent {0}", command.QuestionFeedBackId);
        }

        //post resource status
        [HttpPost("isCheck")]
        public async Task<IActionResult> Status([FromBody] Status status)
        {

            // bool result = await _userRepository.PostNote(user);

            await _userRepository.UserResFind(status);
            return Ok();
        }

        //post user profile
        [HttpPost("UploadsProfilePic")]
        public async Task<IActionResult> UploadsProfilePic()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            try
            {
                foreach (var formFile in files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./wwwroot/image", formFile.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await formFile.CopyToAsync(stream);
                    Console.WriteLine("file uploaded" + formFile.FileName);

                }
                Console.WriteLine("file uploaded" + files.Count);
                return Ok(new { count = files.Count });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        // PUT: api/values/5
        //update user profile
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            // ObjectId id = new ObjectId(_id);
            if (ModelState.IsValid)
            {
                bool result = await _userRepository.FindNote(id);
                if (result)
                {
                    await _userRepository.Update(id, user);
                }
                else
                {
                    await _userRepository.Create(id, user);

                }
                return Ok(user);
            }
            return BadRequest("Invalid Format");
        }
        // DELETE: api/values/5
        //delete user profile
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // ObjectId id = new ObjectId(_id);

            bool result = await _userRepository.FindNote(id);
            if (result)
            {
                await _userRepository.Delete(id);
                return Ok($"note with id : {id} deleted succesfully");
            }
            else
            {
                return NotFound($"Note with {id} not found.");
            }

        }
    }
}