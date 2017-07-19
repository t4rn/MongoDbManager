# MongoDbManager
MongoDB implementation in .NET application.

## Installing MongoDB as Windows Service:
1. Install MongoDB on local drive, e.g. C:\MongoDB\
2. Create folders **data** i **log** in the main directory.
3. Create config file (e.g. **mongod.cfg**):
```
systemLog:
    destination: file
    path: c:\MongoDB\log\mongod.log
storage:
    dbPath: c:\MongoDB\data\db
```

4. Install service:
```
C:\MongoDB\bin\mongod --config C:\MongoDB\mongod.cfg --install
```
