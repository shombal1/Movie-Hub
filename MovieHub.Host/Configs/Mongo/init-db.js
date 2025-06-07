db = db.getSiblingDB('MovieHub');

// Collections
db.createCollection("Users");
db.createCollection("Media");
db.createCollection("MediaBasket");
db.createCollection("Seasons");
db.createCollection("AdditionMediaInfo");
db.createCollection("DomainEvents");
db.createCollection("MovieRequests");

// Indexes
db.Users.createIndex({_id: 1});

db.Media.createIndex({_id: 1});
db.Media.createIndex({releasedYearAt: 1});
db.Media.createIndex({genres: 1});
db.Media.createIndex({countries: 1});

db.MediaBasket.createIndex({userId: 1});
db.MediaBasket.createIndex({mediaId: 1});
db.MediaBasket.createIndex({userId: 1, mediaId: 1}, { unique: true });

db.Seasons.createIndex({seriesId: 1});
db.AdditionMediaInfo.createIndex({mediaId: 1});
db.DomainEvents.createIndex({_id: 1});
db.MovieRequests.createIndex({_id: 1});