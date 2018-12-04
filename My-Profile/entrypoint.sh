#!/bin/bash
set -e
run_cmd="dotnet run"
./wait-for-it.sh -t 0 mongop:27019 -- echo "mongodb is up"
>&2 echo "MongoDB is Up"
exec $run_cmd
