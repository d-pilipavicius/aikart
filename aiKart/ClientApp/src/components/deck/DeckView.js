import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import {
  addCardToDeck,
  editCardInDeck,
  deleteCardFromDeck,
} from "../../app/state/deck/decksSlice";
import AddCardModal from "../card/AddCardModal";
import EditCardModal from "../card/EditCardModal";
import { Card, CardBody, CardTitle, CardText, Button } from "reactstrap";

const DeckView = () => {
  const dispatch = useDispatch();
  const { deckName } = useParams();
  const [showModal, setShowModal] = useState(false);
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [editingCard, setEditingCard] = useState(null);

  const deck = useSelector((state) =>
    state.decks.decks.find((deck) => deck.name === deckName)
  );

  const toggleAddModal = () => {
    setShowModal(!showModal);
  };

  const toggleEditModal = () => {
    setEditModalOpen(!editModalOpen);
  };

  const addCard = (question, answer) => {
    dispatch(addCardToDeck({ deckName, card: { question, answer } }));
    toggleAddModal();
  };

  const editCard = (index, question, answer) => {
    dispatch(
      editCardInDeck({
        deckName,
        cardIndex: index,
        newCard: { question, answer },
      })
    );
    toggleEditModal();
  };

  const openEditModal = (index, card) => {
    setEditingCard({ index, ...card });
    setEditModalOpen(true);
  };

  const deleteCard = (cardIndex) => {
    if (window.confirm("Are you sure you want to delete this card?")) {
      dispatch(deleteCardFromDeck({ deckName, cardIndex }));
    }
  };

  return (
    <div>
      <h2 className="text-dark mb-3">{deckName}</h2>
      <button className="btn btn-primary mb-3" onClick={toggleAddModal}>
        Add Card
      </button>
      {deck &&
        deck.cards.map((card, index) => (
          <Card key={index} className="mb-3">
            <CardBody>
              <CardTitle tag="h5">Question</CardTitle>
              <CardText>{card.question}</CardText>
              <CardTitle tag="h5">Answer</CardTitle>
              <CardText>{card.answer}</CardText>
              <Button
                style={{ marginRight: "1rem" }}
                color="warning"
                onClick={() => openEditModal(index, card)}
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
        toggleModal={toggleAddModal}
        addCard={addCard}
      />
      <EditCardModal
        isOpen={editModalOpen}
        toggle={toggleEditModal}
        card={editingCard}
        editCard={editCard}
      />
    </div>
  );
};

export default DeckView;
