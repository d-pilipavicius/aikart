import React, { useState, useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams, useNavigate } from "react-router-dom";
import { Button, Container } from "reactstrap";
import { fetchDeckById } from "../app/state/deck/decksSlice";
import './DeckPractice.css';

const DeckPractice = () => {
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


  if (!deck) {
    // Handle the case where deck is not loaded yet
    return <div>Loading...</div>;
  }


  return (
    <div
      style={{ minHeight: "60vh", display: "flex", flexDirection: "column" }}
    >
      {currentCardObject && (
        <div className="card-container" onClick={toggleFlip}>
          <div className={`card-flipper ${isFlipped ? "flip" : ""}`}>
            <div className="card-face card-front">
              <h1>{currentCardObject.question}</h1>
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
