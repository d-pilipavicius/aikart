import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import {
  addCard as addCardToDeck,
  updateCard as updateCardInDeck,
  deleteCard as deleteCardFromDeck,
} from "../../app/state/card/cardsSlice";
import { fetchDecks, updateDeck } from "../../app/state/deck/decksSlice";
import { fetchDecksByUser } from "../../app/state/user/userDecksSlice";
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
  const user = useSelector((state) => state.users.currentUser);
  const [deckName, setDeckName] = useState("");
  const [deckDescription, setDeckDescription] = useState("");

  const deck = useSelector((state) =>
    state.userDecks.userDecks.find((deck) => deck.id === parseInt(deckId))
  );

  // Check if the current user is the deck's creator
  const isCreator = user.id === deck.creatorId;

  const toggleAddModal = () => {
    setShowModal(!showModal);
  };

  const toggleEditModal = () => {
    setEditModalOpen(!editModalOpen);
  };

  const addCard = (question, answer) => {
    const cardDto = { deckId: deck.id, question, answer };
    dispatch(addCardToDeck(cardDto)).then(() => {
      dispatch(fetchDecksByUser(user.id));
    });
    toggleAddModal();
  };

   const [isPublic, setIsPublic] = useState(false);

   useEffect(() => {
     if (deck) {
      console.log(deck.isPublic);
       setIsPublic(deck.isPublic);
     }
   }, [deck]);

   const togglePrivacy = () => {
     const newPrivacyStatus = !isPublic;
     setIsPublic(newPrivacyStatus);

     const updatedDeckDto = {
       name: deck.name,
       description: deck.description,
       creatorName: user.name,
       cards: deck.cards,
       isPublic: newPrivacyStatus,
     };

     dispatch(updateDeck({ deckId: deck.id, deckDto: updatedDeckDto }));
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

  if (!deck) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      {isCreator && (
        <div>
          <button
            className={`btn ${isPublic ? "btn-danger" : "btn-success"}`}
            onClick={togglePrivacy}
          >
            {isPublic ? "Make Private" : "Make Public"}
          </button>
        </div>
      )}
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
