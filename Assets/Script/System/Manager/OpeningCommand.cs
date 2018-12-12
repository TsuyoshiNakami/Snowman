using System.Collections;
using System.Collections.Generic;

public enum OpeningCommandType
{
    Message,
    Timeline,
    Input,
    ToGame,
    Wait,
    Method,
    PlaySound,
}

public enum OpeningCommandMode
{
    Wait,
    Through
}
public struct OpeningCommand
{
    public OpeningCommandType type;
    public List<string> msg;
    public OpeningCommandMode mode;

    public OpeningCommand(OpeningCommandType _type, List<string> _msg)
    {
        type = _type;
        msg = _msg;
        mode = OpeningCommandMode.Wait;
    }
    public OpeningCommand(OpeningCommandType _type, List<string> _msg, OpeningCommandMode _mode)
    {
        type = _type;
        msg = _msg;
        mode = _mode;
    }


    public OpeningCommand(OpeningCommandType _type, string _msg)
    {
        type = _type;
        List<string> tmp = new List<string>();
        tmp.Add(_msg);
        msg = tmp;
        mode = OpeningCommandMode.Wait;
    }
    public OpeningCommand(OpeningCommandType _type, string _msg, OpeningCommandMode _mode)
    {
        type = _type;
        List<string> tmp = new List<string>();
        tmp.Add(_msg);
        msg = tmp;
        mode = _mode;
    }
    public OpeningCommand(OpeningCommandType _type)
    {
        type = _type;
        msg = new List<string>();
        mode = OpeningCommandMode.Wait;
    }
}