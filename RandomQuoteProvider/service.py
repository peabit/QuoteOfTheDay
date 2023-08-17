from concurrent import futures
import grpc
import requests

from RandomQuoteProvider_pb2 import Quote

import RandomQuoteProvider_pb2_grpc

class RandomQuoteProvider(RandomQuoteProvider_pb2_grpc.RandomQuoteProviderServicer):
    def Get(self, empty, context):
        response = requests.get("https://quote-garden.onrender.com/api/v3/quotes/random")

        if not response.ok:
            return

        quote = response.json()["data"][0]

        return Quote(text=quote["quoteText"], author=quote["quoteAuthor"])
    
server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))

RandomQuoteProvider_pb2_grpc.add_RandomQuoteProviderServicer_to_server(
    RandomQuoteProvider(),server
)

server.add_insecure_port("[::]:50051")
server.start()
server.wait_for_termination()