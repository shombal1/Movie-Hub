rs.initiate(
    {
        _id: "shard2_rs",
        members: [
            { _id : 0, host : "shardsvr2-1:27017" },
            { _id : 1, host : "shardsvr2-2:27017" },
            { _id : 2, host : "shardsvr2-3:27017" }
        ]
    }
)