import React, { useState, useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams, useNavigate } from "react-router-dom";
import { Button, Container } from "reactstrap";
import { fetchDeckById } from "../app/state/deck/decksSlice";
import { updateRepetitionInterval } from "../app/state/card/cardsSlice";
import {
  incrementAnswerCount,
  incrementSolveCount,
} from "../app/state/user/userDecksSlice";
import "./DeckPractice.css";

const DeckReview = () => {
  const { deckId } = useParams();
  const decks = useSelector((state) => state.userDecks.userDecks);
  const user = useSelector((state) => state.users.currentUser);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    dispatch(fetchDeckById(deckId));
  }, [dispatch, deckId]);

  const [currentCard, setCurrentCard] = useState(0);
  const [counter, setCounter] = useState(0);

  const [isAnswered, setAnswered] = useState(false);

  const deck = decks.find((deck) => deck.id === parseInt(deckId));

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

  const filteredCards = deck?.cards.filter((card) => {
    return (
      card.state === "Unanswered" ||
      Date.now() -
        (card.intervalInDays * 24 * 60 * 60 * 1000 +
          new Date(card.lastRepetition).getTime()) >
        0
    );
  });

  const [remainingUnansweredCards, setRemainingUnansweredCards] = useState(
    filteredCards || []
  );

  const currentCardObject =
    remainingUnansweredCards && remainingUnansweredCards[currentCard];

  const handleNextCard = (difficulty) => () => {
    if (remainingUnansweredCards.length > 0) {
      dispatch(
        updateRepetitionInterval({
          cardId: currentCardObject.id,
          stateValue: difficulty,
        })
      );
      dispatch(
        incrementAnswerCount({
          userId: user.id,
          deckId: deckId,
          stateValue: difficulty,
        })
      );
      setIsFlipped(false);

      setTimeout(() => {
        // If "Hard" is selected, move the card to the back of the list
        if (difficulty === "Unanswered") {
          setRemainingUnansweredCards((prevCards) => [
            ...prevCards.slice(1),
            prevCards[0],
          ]);
        } else {
          setRemainingUnansweredCards((prevCards) => prevCards.slice(1));
        }

        setCurrentCard(0);
        setCounter(counter + 1);
        setAnswered(false);
      }, 500);
    }
  };

  const handleFinish = () => {
    dispatch(incrementSolveCount({ userId: user, deckId: deckId }));
    navigate("/home");
  };

  if (!deck) {
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
          <p>Answered: {counter}</p>
        </div>
        <div className="stat-box">
          <i className="fas fa-layer-group icon"></i>
          <p>Remaining: {remainingUnansweredCards.length}</p>
        </div>
      </Container>

      <Container className="d-flex justify-content-center">
        {remainingUnansweredCards.length > 0 && !isAnswered && (
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
        {remainingUnansweredCards.length > 0 && isAnswered && (
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
        {remainingUnansweredCards.length === 0 && (
          <Button
            style={{ marginRight: "5px" }}
            color="primary"
            onClick={handleFinish}
          >
            Finish
          </Button>
        )}
      </Container>
    </div>
  );
};

export default DeckReview;
