"""A prototype and testing client."""
import asyncio
import logging
import struct

import fitness_pb2

async def read_with_length_prefix(reader):
    len_bytes = await reader.readexactly(4)
    payload_len = struct.unpack(">L", len_bytes)[0]
    payload = await reader.readexactly(payload_len)
    return payload_len, payload


async def write_with_length_prefix(writer, message):
    payload_len = len(message)
    writer.write(struct.pack(">L", payload_len))
    writer.write(message)


async def tcp_echo_client(message):
    reader, writer = await asyncio.open_connection("127.0.0.1", 5051)

    print(f"Send: {message!r}")
    await write_with_length_prefix(writer, message.encode())
    await writer.drain()

    payload_len, payload = await read_with_length_prefix(reader)
    print(f"Received: {payload.decode()!r}")

    print("Close the connection")
    writer.close()
    await writer.wait_closed()


if __name__ == "__main__":
    logging.basicConfig()
    asyncio.run(tcp_echo_client("Hello World!"))