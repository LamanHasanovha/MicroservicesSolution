#!/bin/bash

echo "Starting the microservices environment..."
docker-compose down
docker-compose up -d

echo "Waiting for services to start up..."
sleep 30

echo "Running load tests..."

echo "-----------------------------------"
echo "Test 1: Round Robin - Light Load"
dotnet run --project ./LoadTest roundrobin 100

echo "-----------------------------------"
echo "Test 2: Least Connection - Light Load"
dotnet run --project ./LoadTest leastconnection 100

echo "-----------------------------------"
echo "Test 3: Sticky Sessions - Light Load"
dotnet run --project ./LoadTest stickysessions 100

echo "-----------------------------------"
echo "Test 4: Round Robin - Medium Load"
dotnet run --project ./LoadTest roundrobin 500

echo "-----------------------------------"
echo "Test 5: Least Connection - Medium Load"
dotnet run --project ./LoadTest leastconnection 500

echo "-----------------------------------"
echo "Test 6: Sticky Sessions - Medium Load"
dotnet run --project ./LoadTest stickysessions 500

echo "-----------------------------------"
echo "Test 7: Round Robin - Heavy Load"
dotnet run --project ./LoadTest roundrobin 1000

echo "-----------------------------------"
echo "Test 8: Least Connection - Heavy Load"
dotnet run --project ./LoadTest leastconnection 1000

echo "-----------------------------------"
echo "Test 9: Sticky Sessions - Heavy Load"
dotnet run --project ./LoadTest stickysessions 1000

echo "All tests completed!"