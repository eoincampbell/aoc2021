using Aoc2021.Puzzles.Day16Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day16 : Puzzle
    {
        public override int Day => 16;
        public override string Name => "Packet Decoder";
        protected override object RunPart1() => Process();      //984
        protected override object RunPart2() => _part2Answer;   //1015320896946

        public Day16()
            : base("Inputs/Day16.txt")
            //: base("Inputs/Day16Sample.txt")
            //: base("Inputs/Day16Ian.txt")
        {
        }

        private long _part1Answer;
        private long _part2Answer;
        private readonly bool _printOutput = false;
        public object Process()
        {
            foreach (var line in PuzzleInput)
            {
                var bits = string.Concat(line.ToCharArray().Select(c => c.ToBinary()));
                var bsr = new BitStreamReader(bits);
                var message = bsr.CreateMessage();
                PrintOutput(line, message);
                _part1Answer = message.VersionTotal;
                _part2Answer = message.ValueTotal;
            }
            return _part1Answer;
        }

        private void PrintOutput(string line, Message message)
        {
            if (!_printOutput) return;
            Console.WriteLine("##################################################################");
            Console.WriteLine($"Input: {line}");
            Console.WriteLine(message);
            Console.WriteLine($"VersionSum: {message.VersionTotal}");
            Console.WriteLine($"Value:      {message.ValueTotal}");
        }
    }

    namespace Day16Extensions
    {
        internal class BitStreamReader
        {
            private readonly string _bits;
            private int _ptr;

            public BitStreamReader(string bits) => _bits = bits;

            private string ReadBits(int len)
            {
                _ptr += len;
                return _bits[(_ptr-len).._ptr];
            }

            private long ReadLong(int len) => ReadBits(len).ToInt64();

            private int ReadInt(int len) => ReadBits(len).ToInt32();

            public Message CreateMessage()
            {
                try
                {
                    long ver = ReadLong(3), typ = ReadLong(3);      //3 bits for version, 3 bits for type
                    var msg = new Message(ver, typ);
                    if (typ == 4)
                        PopulateLiteralPacket(ref msg);
                    else
                        PopulateOperatorPacket(ref msg);
                    return msg;
                }
                catch
                {
                    return null!; //if something fails, we're probably just at the end of the stream
                }
            }

            private void PopulateOperatorPacket(ref Message msg)
            {
                msg.Messages = new List<Message>();
                var lengthType = ReadInt(1);                    //1 bit for length type
                var len = ReadInt(lengthType == 0 ? 15 : 11);   //this decides whether the next chunk is 15 or 11 bits

                if (lengthType == 0)                            //and also what mechanism you'll read the subsequent blocks
                    PopulateVariableLengthOperatorPacket(ref msg, len);
                else
                    PopulateFixedLengthOperatorPacket(ref msg, len);
            }

            private void PopulateFixedLengthOperatorPacket(ref Message msg, int len)
            {
                //len is a count of how many messages to create
                for (int i = 0; i < len; i++)
                {
                    var innermsg = CreateMessage();
                    if (innermsg != null)
                        msg?.Messages?.Add(innermsg);
                }
            }

            private void PopulateVariableLengthOperatorPacket(ref Message msg, int len)
            {
                //len is the length of the next section, keep creating message from it until it's consumed
                int tempptr = _ptr;
                while (_ptr < tempptr + len)
                {
                    var innermsg = CreateMessage();
                    if (innermsg == null) break;
                    msg?.Messages?.Add(innermsg);
                }
                _ptr = tempptr + len;
            }

            private void PopulateLiteralPacket(ref Message msg)
            {
                //keep reading 5 bit chunks in the form of XNNNN until X == 0
                //Concatenate all the NNNN segments to reform the value
                var value = "";
                var header = 1;
                while (header != 0)
                {
                    header = ReadInt(1);         //check the leading bit to see if this is a terminal chunk
                    value += ReadBits(4);        //grab the chunk value + append it to the overall value
                }
                msg.LocalValue = value.ToInt64();
            }
        }

        internal record Message(long Version, long Type)
        {
            public bool IsLiteralPacket => Type == 4;
            public long LocalValue { get; set; }
            public List<Message>? Messages { get; set; }

            public long VersionTotal => (Messages == null)
                ? Version
                : Version + Messages.Sum(m => m.VersionTotal);

            public long ValueTotal => Type switch
                    {
                        0 => (Messages?.Any() == true) ? Messages.Sum(m => m.ValueTotal) : 0,
                        1 => (Messages?.Any() == true) ? Messages.Product(m => m.ValueTotal) : 0,
                        2 => (Messages?.Any() == true) ? Messages.Min(m => m.ValueTotal) : 0,
                        3 => (Messages?.Any() == true) ? Messages.Max(m => m.ValueTotal) : 0,
                        4 => LocalValue,
                        5 => (Messages?.Count == 2 && Messages[0].ValueTotal > Messages[1].ValueTotal) ? 1 : 0,
                        6 => (Messages?.Count == 2 && Messages[0].ValueTotal < Messages[1].ValueTotal) ? 1 : 0,
                        7 => (Messages?.Count == 2 && Messages[0].ValueTotal == Messages[1].ValueTotal) ? 1 : 0,
                        _ => 0,
                    };

            public override string ToString() => Dump(0);

            private string Dump (int level)
            {
                var val = Environment.NewLine +
                    string.Concat(Enumerable.Repeat("  ", level)) + //nesting
                    $"Ver: {Version} | " +
                    $"Pkt: {(IsLiteralPacket ? "Literal " : "Operator")} | " +
                    $"Typ: {Type}" +
                    $"{(IsLiteralPacket ? $" | Val: {LocalValue}" : "")}";

                return Messages == null
                    ? val
                    : val + string.Concat(Messages.Select(x => x.Dump(level + 1)));
            }
        }

        internal static class Extensions
        {
            internal static long ToInt64(this string bits) => Convert.ToInt64(bits, 2);
            internal static int ToInt32(this string bits) => Convert.ToInt32(bits, 2);
            internal static string ToBinary(this char hex) => hex switch
            {
                '0' => "0000",  '1' => "0001",  '2' => "0010",  '3' => "0011",
                '4' => "0100",  '5' => "0101",  '6' => "0110",  '7' => "0111",
                '8' => "1000",  '9' => "1001",  'A' => "1010",  'B' => "1011",
                'C' => "1100",  'D' => "1101",  'E' => "1110",  'F' => "1111",
                _ => throw new ArgumentOutOfRangeException(nameof(hex))
            };
        }
    }
}