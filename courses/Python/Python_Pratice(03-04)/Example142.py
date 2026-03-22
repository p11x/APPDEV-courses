# Example142.py
# Topic: Struct for Binary Data


# ============================================================
# Example 1: Basic Pack/Unpack
# ============================================================
print("=== Basic Pack/Unpack ===")

import struct

data = struct.pack("iii", 1, 2, 3)
print(f"Packed: {data}")
print(f"Unpacked: {struct.unpack('iii', data)}")


# ============================================================
# Example 2: Different Types
# ============================================================
print("\n=== Different Types ===")

import struct

packed = struct.pack("i b d", 42, 1, 3.14)
print(f"Packed: {packed}")

unpacked = struct.unpack("i b d", packed)
print(f"Unpacked: {unpacked}")


# ============================================================
# Example 3: Binary File Format
# ============================================================
print("\n=== BMP Header ===")

import struct

bmp_header = struct.pack("<HHIIHH", 
    0x4D42,
    100,
    0,
    0,
    54,
    1
)

print(f"BMP Signature: {bmp_header[:2]}")
print(f"File size: {struct.unpack('<I', bmp_header[2:6])[0]}")


# ============================================================
# Example 4: Network Packets
# ============================================================
print("\n=== Network Packet ===")

import struct

def create_packet(seq, ack, data):
    header = struct.pack("!II", seq, ack)
    return header + data.encode()

def parse_packet(packet):
    seq, ack = struct.unpack("!II", packet[:8])
    data = packet[8:].decode()
    return seq, ack, data

pkt = create_packet(100, 200, "Hello")
seq, ack, data = parse_packet(pkt)
print(f"Seq: {seq}, Ack: {ack}, Data: {data}")


# ============================================================
# Example 5: Arrays and Counts
# ============================================================
print("\n=== Arrays ===")

import struct

count = 3
values = [10, 20, 30]
packed = struct.pack("I" * count, *values)
print(f"Packed: {packed}")

unpacked = struct.unpack("I" * count, packed)
print(f"Unpacked: {unpacked}")


# ============================================================
# Example 6: Real-World: File Metadata
# ============================================================
print("\n=== Real-World: File Record ===")

import struct

class FileRecord:
    FORMAT = "50s 50s I I"
    SIZE = struct.calcsize(FORMAT)
    
    def __init__(self, filename, content, size, checksum):
        self.filename = filename
        self.content = content
        self.size = size
        self.checksum = checksum
    
    def pack(self):
        return struct.pack(self.FORMAT, 
            self.filename.encode()[:50],
            self.content.encode()[:50],
            self.size,
            self.checksum
        )
    
    @classmethod
    def unpack(cls, data):
        fn, c, s, ch = struct.unpack(cls.FORMAT, data)
        return cls(fn.decode().strip('\x00'), 
                   c.decode().strip('\x00'), s, ch)

rec = FileRecord("test.txt", "content", 1000, 12345)
packed = rec.pack()
print(f"Packed: {packed}")

rec2 = FileRecord.unpack(packed)
print(f"Unpacked: {rec2.filename}, {rec2.size}")
