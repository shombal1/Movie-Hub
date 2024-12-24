rs.initiate(
    {
        _id: "shard2_rs",
        members: [
            { _id : 0, host : "shardsvr2_1:27017" },
            { _id : 1, host : "shardsvr2_2:27017" },
            { _id : 2, host : "shardsvr2_3:27017" }
        ]
    }
)