import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import {
  addCardToDeck,
  editCardInDeck,
  deleteCardFromDeck,
} from "../../features/deck/decksSlice";
import AddCardModal from "../card/AddCardModal";
import { Card, CardBody, CardTitle, CardText, Button } from "reactstrap";

const DeckView = () => {
  const dispatch = useDispatch();
  const { deckName } = useParams();
  const [showModal, setShowModal] = useState(false);
  const deck = useSelector((state) =>
    state.decks.decks.find((deck) => deck.name === deckName)
  );

  const toggleModal = () => {
    setShowModal(!showModal);
  };

  const addCard = (question, answer) => {
    dispatch(addCardToDeck({ deckName, card: { question, answer } }));
    toggleModal();
  };

  const editCard = (cardIndex) => {
    const newQuestion = prompt("New question:", deck.cards[cardIndex].question);
    const newAnswer = prompt("New answer:", deck.cards[cardIndex].answer);
    dispatch(
      editCardInDeck({
        deckName,
        cardIndex,
        newCard: { question: newQuestion, answer: newAnswer },
      })
    );
  };

  const deleteCard = (cardIndex) => {
    if (window.confirm("Are you sure you want to delete this card?")) {
      dispatch(deleteCardFromDeck({ deckName, cardIndex }));
    }
  };

  return (
    <div>
      <h2 className="text-dark mb-3">{deckName}</h2>
      <button className="btn btn-primary mb-3" onClick={toggleModal}>
        Add Card
      </button>
      {deck &&
        deck.cards.map((card, index) => (
          <Card key={index} className="mb-3">
            <CardBody>
              <CardTitle tag="h5" className="mb-2">
                Question
              </CardTitle>
              <CardText className="mb-3">{card.question}</CardText>
              <CardTitle tag="h5" className="mb-2">
                Answer
              </CardTitle>
              <CardText className="mb-3">{card.answer}</CardText>
              <Button
                style={{ marginRight: "1rem" }}
                color="warning"
                onClick={() => editCard(index)}
              >
                Edit
              </Button>
              <Button color="danger" onClick={() => deleteCard(index)}>
                Delete
              </Button>
            </CardBody>
          </Card>
        ))}
      <AddCardModal
        showModal={showModal}
        toggleModal={toggleModal}
        addCard={addCard}
      />
    </div>
  );

};

export default DeckView;
