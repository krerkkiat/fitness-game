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


async def handle_client(reader, writer):
    payload_len, payload = await read_with_length_prefix(reader)

    addr = writer.get_extra_info("peername")
    message = payload.decode()

    print(f"Received {message!r} from {addr!r}")

    print(f"Send: {message!r}")
    await write_with_length_prefix(writer, payload)
    await writer.drain()

    print("Close the connection")
    writer.close()
    await writer.wait_closed()


async def main():
    server = await asyncio.start_server(handle_client, "0.0.0.0", 5051)

    addrs = ", ".join(str(sock.getsockname()) for sock in server.sockets)
    print(f"Serving on {addrs}")

    async with server:
        await server.serve_forever()

if __name__ == "__main__":
    logging.basicConfig()
    asyncio.run(main())
