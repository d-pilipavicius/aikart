import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { updateDeck, fetchDecks } from "../../app/state/deck/decksSlice";
import {
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Button,
  Input,
  Form,
} from "reactstrap";

const EditDeckModal = ({ deck, isOpen, toggle }) => {
  const [newName, setNewName] = useState(deck.name);
  const [newDescription, setNewDescription] = useState(deck.description);
  const dispatch = useDispatch();

  const handleSave = (event) => {
    event.preventDefault();
    const deckId = deck.id;
    dispatch(
      updateDeck({
        deckId,
        deckDto: { name: newName, description: newDescription },
      })
    ).then(() => {
      dispatch(fetchDecks());
    });
    toggle();
  };

  const handleKeyPress = (event) => {
    if (event.key === "Enter") {
      handleSave(event);
    }
  };

  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Edit Deck</ModalHeader>
      <Form onSubmit={handleSave}>
        <ModalBody>
          <Input
            type="text"
            value={newName}
            onChange={(e) => setNewName(e.target.value)}
            onKeyPress={handleKeyPress}
          />
          <Input
            type="text"
            value={newDescription}
            onChange={(e) => setNewDescription(e.target.value)}
            className="mt-2"
            onKeyPress={handleKeyPress}
          />
        </ModalBody>
        <ModalFooter>
          <Button color="success" type="submit">
            Save
          </Button>
          <Button color="secondary" onClick={toggle}>
            Cancel
          </Button>
        </ModalFooter>
      </Form>
    </Modal>
  );
};

export default EditDeckModal;
