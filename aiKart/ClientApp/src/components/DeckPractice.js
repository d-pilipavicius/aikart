import React, { useState, useEffect} from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams, useNavigate } from "react-router-dom";
import { Button, Container } from "reactstrap";
import { fetchDecks } from "../app/state/deck/decksSlice";

const DeckPractice = () => {
  const { deckId } = useParams();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    dispatch(fetchDecks());
  }, [dispatch]);


  const deck = useSelector((state) =>
    state.decks.decks.find((deck) => deck.id === parseInt(deckId))
  );

  
  const [currentCard, setCurrentCard] = useState(0);
  
  const currentCardObject = deck && deck.cards[currentCard];

  const [isAnswered, setAnswered] = useState(false);

  return (
    <div
      style={{ minHeight: "60vh", display: "flex", flexDirection: "column" }}
    >
      {currentCardObject && (
        <div>
          <h1>{currentCardObject.question}</h1>
          <hr></hr>
          {isAnswered && <h1>{currentCardObject.answer}</h1>}
        </div>
      )}

      <Container
        className="d-flex justify-content-center align-items-end"
        style={{ marginTop: "auto" }}
      >
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            justifyContent: "center",
            margin: "5px",
          }}
        >
          <p>Answered</p>
          <p>{currentCard}</p>
        </div>
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            justifyContent: "center",
            margin: "5px",
          }}
        >
          <p>Remaining</p>
          <p>{deck.cards.length - currentCard}</p>
        </div>
      </Container>

      <Container className="d-flex justify-content-center">
        {!isAnswered && (
          <Button
            style={{ marginRight: "5px" }}
            color="primary"
            onClick={() => {
              setAnswered(true);
            }}
          >
            Show answer
          </Button>
        )}
        {currentCard < deck.cards.length - 1 && isAnswered && (
          <div>
            <Button
              style={{ marginRight: "5px" }}
              color="danger"
              onClick={() => {
                setCurrentCard(currentCard + 1);
                setAnswered(false);
              }}
            >
              Hard
            </Button>
            <Button
              style={{ marginRight: "5px" }}
              color="warning"
              onClick={() => {
                setCurrentCard(currentCard + 1);
                setAnswered(false);
              }}
            >
              Decent
            </Button>
            <Button
              color="success"
              onClick={() => {
                setCurrentCard(currentCard + 1);
                setAnswered(false);
              }}
            >
              Easy
            </Button>
          </div>
        )}
        {currentCard === deck.cards.length - 1 && isAnswered && (
          <Button
            style={{ marginRight: "5px" }}
            color="primary"
            onClick={() => navigate("/home")}
          >
            Finish
          </Button>
        )}
      </Container>
    </div>
  );
};

export default DeckPractice;
