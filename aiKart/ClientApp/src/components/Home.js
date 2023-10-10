import React, { useEffect } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { fetchDecks } from "../app/state/deck/decksSlice";
import { useDispatch } from "react-redux";
import { Card, CardText, CardBody, CardHeader } from "reactstrap";

const Home = () => {
  const decks = useSelector((state) => state.decks.decks);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(fetchDecks());
  }, [dispatch]);

  return (
    <div>
      <h1>Welcome to AiKart!</h1>
      <h5>
        Select a deck you want to practice or create a new one by pressing{" "}
        <strong>Create Deck</strong>.
      </h5>
      <h5>
        Press <strong>Manage Decks</strong> to edit or delete existing decks.
      </h5>
      <br />

      <div className="row">
        {decks.map((deck, index) => (
          <div className="col-md-4" key={index}>
            <Card
              className="mb-4"
              onClick={() => navigate(`/practice/${deck.id}`)}
              style={{ cursor: "pointer" }}
            >
              <CardHeader className="bg-primary bg-gradient text-light">
                {deck.name}
              </CardHeader>
              <CardBody>
                <CardText>{deck.description}</CardText>
              </CardBody>
            </Card>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Home;
