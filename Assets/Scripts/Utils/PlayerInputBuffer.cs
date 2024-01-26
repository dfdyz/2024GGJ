
using System.Collections;

public class PlyerInputBuffer
{

    InputClip[] buffer;

    public PlyerInputBuffer(int size)
    {
        buffer = new InputClip[size];

        for (int i = 0; i < size; ++i)
        {
            buffer[i].Init();
        }
    }

    public void Put(int atSlot, InputType inputType)
    {
        if(!buffer[atSlot].isFull()) buffer[atSlot].types[buffer[atSlot].count++] = inputType;
    }

    public int GetBufferedCount(int atSlot)
    {
        return buffer[atSlot].count;
    }

    public InputType GetBufferedType(int atSlot, int idx)
    {
        return buffer[atSlot].types[idx];
    }

    public void Clear()
    {
        for(int i = 0; i < buffer.Length; ++i)
        {
            buffer[i].Clear();
        }
    }

    public override string ToString()
    {
        string str = string.Format("[{0}] [{1}] [{2}] [{3}]\n",
                buffer[0].count > 0 ? Type2Str(buffer[0].types[0]) : "-",
                buffer[1].count > 0 ? Type2Str(buffer[1].types[0]) : "-",
                buffer[2].count > 0 ? Type2Str(buffer[2].types[0]) : "-",
                buffer[3].count > 0 ? Type2Str(buffer[3].types[0]) : "-"
            );
        str += string.Format("[{0}] [{1}] [{2}] [{3}]",
                buffer[0].count > 1 ? Type2Str(buffer[0].types[1]) : "-",
                buffer[1].count > 1 ? Type2Str(buffer[1].types[1]) : "-",
                buffer[2].count > 1 ? Type2Str(buffer[2].types[1]) : "-",
                buffer[3].count > 1 ? Type2Str(buffer[3].types[1]) : "-"
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

    }

    





}