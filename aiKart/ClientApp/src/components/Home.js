import React, { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import {
  Card,
  CardText,
  CardBody,
  CardHeader,
  CardFooter,
  Button,
} from "reactstrap";
import { fetchDecksByUser } from "../app/state/user/userDecksSlice";

const Home = () => {
  const decks = useSelector((state) => state.userDecks.userDecks);
  const user = useSelector((state) => state.users.currentUser);
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    setIsLoading(true);
    const startTime = new Date();

    const fetchData = async () => {
      await dispatch(fetchDecksByUser(user.id));
      setIsLoading(false);

      const endTime = new Date();
      const fetchDuration = endTime - startTime;
      console.log(`Fetch took ${fetchDuration} milliseconds`);
    };

    if (user) {
      fetchData();
    }
  }, [dispatch, user]);

  const filterCards = (cards) => {
    const now = new Date().getTime();
    return cards.filter(
      (card) =>
        card.state === "Unanswered" ||
        now -
          (card.intervalInDays * 24 * 60 * 60 * 1000 +
            new Date(card.lastRepetition).getTime()) >
          0
    );
  };

  return (
    <div>
      <h1>Welcome to AiKart, {user && user.name}!</h1>
      {isLoading && <p>Loading...</p>}
      {!isLoading && (
        <div>
          <h5>
            Go to the <strong>Store</strong> to get new decks or create one
            yourself by pressing <strong>Manage Decks</strong>.
          </h5>
          <h5>
            See personal deck and card statistics by pressing on your username.
          </h5>
          <h5>
            Once you have decks you can click below to start your daily
            review(s)!
          </h5>
          <br />

          <div className="row">
            {decks.map(
              (deck, index) =>
                filterCards(deck.cards).length > 0 && (
                  <div className="col-md-6" key={index}>
                    <Card
                      className="mb-4"
                      onClick={() => navigate(`/review/${deck.id}`)}
                      style={{ cursor: "pointer" }}
                    >
                      <CardHeader className="bg-primary bg-gradient text-light">
                        {deck.name}
                      </CardHeader>
                      <CardBody>
                        <CardText>{deck.description}</CardText>
                        <div className="text-muted mt-3">
                          Daily review: {filterCards(deck.cards).length} card(s)
                        </div>
                      </CardBody>
                    </Card>
                  </div>
                )
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default Home;
