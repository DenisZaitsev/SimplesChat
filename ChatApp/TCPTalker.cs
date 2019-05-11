using System;

public static class TCPTalker
{
    public void SendMessage(Message message)
    {
        BinaryFormatter bf = new BinaryFormatter();

        //Send name message
        using (MemoryStream memStream = new MemoryStream())
        {
            bf.Serialize(memStream, message);
            netStream.Write(memStream.ToArray(), 0, Convert.ToInt32(memStream.Length));
        }
    }
}
