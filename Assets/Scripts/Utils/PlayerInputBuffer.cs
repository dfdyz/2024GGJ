
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputBuffer
{
    public int bufferedCount { get; private set; } = 0;
    public InputClip[] bufferedInput { get; private set; }
    public HashSet<int> matchedSkill { get; private set; } = new HashSet<int>();
    public HashSet<int> lastMatchedSkill { get; private set; } = new HashSet<int>();

    public PlayerInputBuffer(int size)
    {
        bufferedInput = new InputClip[size];

        for (int i = 0; i < size; ++i)
        {
            bufferedInput[i].Init();
        }
    }

    public void Put(int atSlot, InputType inputType)
    {
        if(!bufferedInput[atSlot].isFull())
        {
            if(bufferedInput[atSlot].count == 1)
            {
                if (bufferedInput[atSlot].types[0] == InputType.Accept)
                {
                    bufferedInput[atSlot].types[1] = bufferedInput[atSlot].types[0];
                    bufferedInput[atSlot].types[0] = inputType;
                }
                else
                {
                    bufferedInput[atSlot].types[1] = inputType;
                }
            }
            else
            {
                bufferedInput[atSlot].types[0] = inputType;
            }

            bufferedInput[atSlot].count++;
        }
        bufferedCount = Math.Max(bufferedCount, atSlot);
    }

    public int GetBufferedCount(int atSlot)
    {
        return bufferedInput[atSlot].count;
    }

    public InputType GetBufferedType(int atSlot, int idx)
    {
        return bufferedInput[atSlot].types[idx];
    }

    public void Clear()
    {
        for(int i = 0; i < bufferedInput.Length; ++i)
        {
            bufferedInput[i].Clear();
        }

        matchedSkill.Clear();
    }

    public override string ToString()
    {
        string str = string.Format("[{0}] [{1}] [{2}] [{3}]\n",
                bufferedInput[0].count > 0 ? Type2Str(bufferedInput[0].types[0]) : "-",
                bufferedInput[1].count > 0 ? Type2Str(bufferedInput[1].types[0]) : "-",
                bufferedInput[2].count > 0 ? Type2Str(bufferedInput[2].types[0]) : "-",
                bufferedInput[3].count > 0 ? Type2Str(bufferedInput[3].types[0]) : "-"
            );
        str += string.Format("[{0}] [{1}] [{2}] [{3}]",
                bufferedInput[0].count > 1 ? Type2Str(bufferedInput[0].types[1]) : "-",
                bufferedInput[1].count > 1 ? Type2Str(bufferedInput[1].types[1]) : "-",
                bufferedInput[2].count > 1 ? Type2Str(bufferedInput[2].types[1]) : "-",
                bufferedInput[3].count > 1 ? Type2Str(bufferedInput[3].types[1]) : "-"
            );
        return str;
    }


    public enum InputType
    {
        MoveL, MoveR, Accept
    }

    string Type2Str(InputType type)
    {
        switch(type)
        {
            case InputType.MoveL: return "L";
            case InputType.MoveR: return "R";
            case InputType.Accept: return "J";
        }

        return " ";
    }



    [Serializable]
    public struct InputClip
    {
        public InputType[] types;
        public int count;

        public bool isFull()
        {
            return count >= 2;
        }

        public void Init()
        {
            types = new InputType[2];
            count = 0;
        }

        public void Clear()
        {
            count = 0;
        }

        public override int GetHashCode()
        {
            if(count == 0) return 0;
            else if(count == 1) return Convert.ToInt32(types[0]) + 1;
            else return Convert.ToInt32(types[0]) + 1 + 10 * Convert.ToInt32(types[1]) + 10;
        }


        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }


        public static bool operator ==(InputClip p1, InputClip p2)
        {
            return p1.GetHashCode() == p2.GetHashCode();
        }

        public static bool operator !=(InputClip p1, InputClip p2)
        {
            return !(p1 == p2);
        }


    }

    





}