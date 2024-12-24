rs.initiate(
    {
        _id: "shard1_rs",
        members: [
            { _id : 0, host : "shardsvr1_1:27017" },
            { _id : 1, host : "shardsvr1_2:27017" },
            { _id : 2, host : "shardsvr1_3:27017" }
        ]
    }
)