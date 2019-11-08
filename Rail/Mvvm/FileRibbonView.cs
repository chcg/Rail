namespace Rail.Mvvm
{
    public class FileRibbonView : AppRibbonView
    {
        public FileRibbonView()
        {
            this.AllowDrop = true;
            
            this.SetEventBinding("PreviewDragEnter", "DragCommand");
            this.SetEventBinding("PreviewDragOver", "DragCommand");
            this.SetEventBinding("PreviewDrop", "DropCommand");
        }
    }
}
