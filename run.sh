#!/bin/bash

set -e
set -o pipefail

dotnet run --project AdventOfCode2024/AdventOfCode2024.csproj "$@"