using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    public interface IInputStreamSender{
        public void OnInputStreamLink(IInputStreamReceiver receiver);
    }
    public interface IInputStreamReceiver{
        public void OnInputStreamLinked(IInputStreamSender receiver);
    }

    public static class InputStreamSystem{
        public static void LinkInputStream(this IInputStreamSender sender,IInputStreamReceiver receiver){
            sender.OnInputStreamLink(receiver);
            receiver.OnInputStreamLinked(sender);
        }
    }
}