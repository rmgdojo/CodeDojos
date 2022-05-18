using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling_Scores
{
    public class Scorer
    {
        private int _score;
        private int _currentFrame;
        private Frame[] _frames = new Frame[10];

        public int Score => _score;

        public void AddFrame(char first, char second, char? third = null)
        {
            Frame current = third is null ? new Frame() { First = first, Second = second }
                                                   : new TenthFrame() { First = first, Second = second, Third = third.Value };
            Frame previous = _currentFrame > 1 ? _frames[_currentFrame - 1] : null;

            current.Previous = previous;
            if (previous != null) previous.Next = current;

            _frames[_currentFrame++] = current;
            CalculateScore();
        }

        private void CalculateScore()
        {
            for (int i = 0; i < _currentFrame; i++)
            {
                Frame frame = _frames[_currentFrame++];
                int frameTotal = frame.GetTotal();

                if (frame is not TenthFrame)
                {
                    if (frame.Second == '/')
                    {
                        if (frame.Next is not null)
                        {
                            if (frame.Next.First != 'X')
                            {
                                frameTotal += 10 + int.Parse(frame.Next.First.ToString());
                            }
                        }
                    }
                    else if (frame.First == 'X')
                    {

                    }
                }
                else
                {
                }

                
            }
        }

        public Scorer()
        {
        }
    }

    public class Frame
    {
        public char First { get; set; }
        public char Second { get; set; }

        public Frame Previous { get; set; }
        public Frame Next { get; set; }

        public virtual int GetTotal()
        {
            bool firstIsNumber = int.TryParse(First.ToString(), out int first);
            bool secondIsNumber = int.TryParse(Second.ToString(), out int second);

            return (firstIsNumber ? first : 0) + (secondIsNumber ? second : 0);
        }
    }

    public class TenthFrame : Frame
    {
        public char Third { get; set; }

        public override int GetTotal()
        {
            bool thirdIsNumber = int.TryParse(Third.ToString(), out int third);
            return base.GetTotal() + (thirdIsNumber ? third : 0);
        }
    }
}
