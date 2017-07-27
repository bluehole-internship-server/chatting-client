using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

namespace chatting_client
{
    class Protocol
    {
        public static Encoding euckr = Encoding.GetEncoding(51949);
        public static Encoding currEncoding = euckr;

        public enum PacketType : short
        {
            LOGIN_REQ,
            LOGIN_ANS,
            CHAT_SEND,
            CHAT_RECV
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct PacketHeader
        {
            public short size;
            public PacketType type;
        }

        public struct PacketLoginReq
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public String user_name;
        }

        public struct PacketLoginAns
        {
            public enum Type : short
            {
                FAIL_DUPLICATE,
                FAIL_TOO_SHORT,
                FAIL_TOO_LONG,
                FAIL_UNKNOWN,
                SUCCESS,
            }

            public Type type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PacketChatSend
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String chat_msg;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PacketChatRecv
        {
            public enum Type : short
            {
                NORMAL,
                NOTICE,
                WHISPER
            }

            public Type type;
            public ushort len_user_name;
            
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String chat_contents;
        }

        public static byte[] PacketToByteArray(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static T ByteArrayToPacket<T>(byte[] byte_array, int len)
        {
            T packet = default(T);
            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.Copy(byte_array, 0, ptr, len);
            packet = (T)Marshal.PtrToStructure(ptr, packet.GetType());
            Marshal.FreeHGlobal(ptr);

            return packet;
        }
        
        public static byte[] RecvFromServer(Socket client, ref PacketHeader header)
        {
            // approximate max length
            byte[] byte_header = new byte[4];
            byte[] byte_packet = new byte[350];

            // HACK?
            Array.Clear(byte_header, 0, 4);
            Array.Clear(byte_packet, 0, 350);

            try
            {
                int ret = 0;
                int len_header = Marshal.SizeOf(header);

                while (ret < len_header)
                {
                    ret += client.Receive(byte_header, ret, len_header - ret, SocketFlags.None);
                }

                header = ByteArrayToPacket<PacketHeader>(byte_header, len_header);

                ret = 0;
                while (ret < header.size)
                {
                    ret += client.Receive(byte_packet, ret, header.size - ret, SocketFlags.None);
                }

                return byte_packet;
            }
             catch (SocketException)
            {
                throw;
            }
        }
    }
    
}
