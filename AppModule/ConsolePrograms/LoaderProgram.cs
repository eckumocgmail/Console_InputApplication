public class LoaderProgram
{
    public System.Collections.Generic.List<string> Dirs = new System.Collections.Generic.List<string>();

    [Label("Добавить")]
    public void Add(string dir)
    {
        Dirs.Add(dir);
    }


    [Label("Удалить")]
    public void Remove([InputNumber()] int index)
    {
        Dirs.RemoveAt(index);
    }
}
