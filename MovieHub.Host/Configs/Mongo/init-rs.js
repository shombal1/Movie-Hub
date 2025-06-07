
const rsConfig = {
    _id: "rs0",
    members: [
        { _id: 0, host: "aaa-movie-hub-items-mongo:27017", priority: 2 },
    ],
};

rs.initiate(rsConfig);

while (!rs.isMaster().ismaster) {
    sleep(1000);
}
