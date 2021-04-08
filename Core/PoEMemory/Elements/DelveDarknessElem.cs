namespace ExileCore.PoEMemory.Elements {
    public class DelveDarknessElement :Element { 
        public int darkness { get { var elem = GetChildFromIndices(0, 0, 2, 0);
                return elem == null || (elem!=null && elem.Text==null)  ? 0 : int.Parse(elem.Text);
            } }
    }
}
