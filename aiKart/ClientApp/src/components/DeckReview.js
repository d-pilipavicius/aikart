import React, { useState, useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams, useNavigate } from "react-router-dom";
import { Button, Container } from "reactstrap";
import { fetchDeckById } from "../app/state/deck/decksSlice";
import { updateRepetitionInterval } from "../app/state/card/cardsSlice";
import "./DeckPractice.css";

const DeckReview = () => {
  const { deckId } = useParams();
  const decks = useSelector((state) => state.userDecks.userDecks);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    dispatch(fetchDeckById(deckId));
  }, [dispatch, deckId]);

  const [currentCard, setCurrentCard] = useState(0);

  const [isAnswered, setAnswered] = useState(false);

  const deck = decks.find((deck) => deck.id === parseInt(deckId));

  const currentCardObject = deck && deck.cards[currentCard];

  const [isFlipped, setIsFlipped] = useState(false);

  // Toggle flip state
  const toggleFlip = () => {
    setIsFlipped(!isFlipped);
  };

  const renderTextWithNewLines = (text) => {
    return text.split("\n").map((line, index, array) => (
      <React.Fragment key={index}>
        {line}
        {index !== array.length - 1 && <br />}
      </React.Fragment>
    ));
  };

  const handleNextCard = (difficulty) => () => {
    dispatch(updateRepetitionInterval({ cardId: currentCardObject.id, stateValue: difficulty}));

    // First, unflip the card
    setIsFlipped(false);

    // Wait for the flip animation to complete
    setTimeout(() => {
      // Update to the next card
      if (currentCard < deck.cards.length - 1) {
        setCurrentCard(currentCard + 1);
      }

      // Reset the answered state for the next card
      setAnswered(false);
    }, 500); // Adjust this duration to match your flip animation time
  };

  if (!deck) {
    // Handle the case where deck is not loaded yet
    return <div>Loading...</div>;
  }

  return (
    <div
      style={{ minHeight: "60vh", display: "flex", flexDirection: "column" }}
    >
      {currentCardObject && (
        <div
          className="card-container"
          onClick={() => {
            toggleFlip();
            setAnswered(true);
          }}
        >
          <div className={`card-flipper ${isFlipped ? "flip" : ""}`}>
            <div className="card-face card-front">
              <h1>{renderTextWithNewLines(currentCardObject.question)}</h1>
            </div>
            <div className="card-face card-back">
              <h1>{currentCardObject.answer}</h1>
            </div>
          </div>
        </div>
      )}
      <Container className="stats-container">
        <div className="stat-box">
          <i className="fas fa-check icon"></i>
          <p>Answered: {currentCard}</p>
        </div>
        <div className="stat-box">
          <i className="fas fa-layer-group icon"></i>
          <p>Remaining: {deck.cards.length - currentCard}</p>
        </div>
      </Container>

      <Container className="d-flex justify-content-center">
        {!isAnswered && (
          <Button
            style={{ marginRight: "5px" }}
            color="primary"
            onClick={() => {
              toggleFlip();
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
              onClick={handleNextCard("Unanswered")}
            >
              Hard
            </Button>
            <Button
              style={{ marginRight: "5px" }}
              color="warning"
              onClick={handleNextCard("PartiallyAnswered")}
            >
              Decent
            </Button>
            <Button color="success" onClick={handleNextCard("Answered")}>
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

export default DeckReview;
