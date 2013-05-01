Glimpse-Linq2Sql
================

Linq2Sql tracing plugin for Glimpse

---

Glimpse.Linq2SQL adds the following Glimpse plugins:

* Linq2SQL

See `glimpse.readme.txt` or http://getglimpse.com for more info

---

Most Linq2SQL context files are generated by a dbml file. 
To enable support for Linq2SQL tracing you have to attach tracers to your context. With a `Local.dbml` file, you have to add the following class:

    public partial class LocalDataContext
    {
        partial void OnCreated()
        {
            Log = Glimpse.Linq2Sql.PluginTextWriter.Instance;
            Connection.StateChange += Glimpse.Linq2Sql.StateChangeHandler.OnStateChange;
        }
    }
