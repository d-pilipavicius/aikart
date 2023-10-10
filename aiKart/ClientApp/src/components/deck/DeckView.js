import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import {
  addCard as addCardToDeck,
  updateCard as updateCardInDeck,
  deleteCard as deleteCardFromDeck,
} from "../../app/state/card/cardsSlice";
import { fetchDecks } from "../../app/state/deck/decksSlice";
import AddCardModal from "../card/AddCardModal";
import EditCardModal from "../card/EditCardModal";
import {
  Card,
  CardBody,
  CardTitle,
  CardText,
  Button,
  CardFooter,
} from "reactstrap";

const DeckView = () => {
  const dispatch = useDispatch();
  const { deckId } = useParams();
  const [showModal, setShowModal] = useState(false);
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [editingCard, setEditingCard] = useState(null);

  const deck = useSelector((state) =>
    state.decks.decks.find((deck) => deck.id === parseInt(deckId))
  );

  const toggleAddModal = () => {
    setShowModal(!showModal);
  };

  const toggleEditModal = () => {
    setEditModalOpen(!editModalOpen);
  };

  const addCard = (question, answer) => {
    const cardDto = { deckId: deck.id, question, answer };
    dispatch(addCardToDeck(cardDto)).then(() => {
      dispatch(fetchDecks());
    });
    toggleAddModal();
  };

  const editCard = (index, question, answer) => {
    const cardId = deck.cards[index].id;
    const cardDto = { question, answer };
    dispatch(updateCardInDeck({ cardId, cardDto })).then(() => {
      dispatch(fetchDecks());
    });
    toggleEditModal();
  };

  const openEditModal = (index, card) => {
    setEditingCard({ index, ...card });
    setEditModalOpen(true);
  };

  const deleteCard = (cardIndex) => {
    const cardId = deck.cards[cardIndex].id;
    if (window.confirm("Are you sure you want to delete this card?")) {
      dispatch(deleteCardFromDeck(cardId)).then(() => {
        dispatch(fetchDecks());
      });
    }
  };

  return (
    <div>
      <h2 className="text-dark mb-3">{deck.name}</h2>
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
            </CardBody>
            <CardFooter>
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
            </CardFooter>
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
