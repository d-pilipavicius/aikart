import React from "react";
import {
  Button,
  Form,
  Input,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
} from "reactstrap";

const CreateDeckModal = ({
  isOpen,
  toggle,
  saveDeck,
  newDeckName,
  setNewDeckName,
  newDeckDescription,
  setNewDeckDescription,
}) => {
  const handleKeyPress = (event) => {
    if (event.key === "Enter") {
      saveDeck(event);
    }
  };

  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Create New Deck</ModalHeader>
      <ModalBody>
        <Form onSubmit={saveDeck}>
          <Input
            type="text"
            placeholder="Deck Name"
            value={newDeckName}
            onChange={(e) => setNewDeckName(e.target.value)}
            onKeyPress={handleKeyPress}
          />
          <Input
            type="text"
            placeholder="Deck Description"
            value={newDeckDescription}
            onChange={(e) => setNewDeckDescription(e.target.value)}
            className="mt-2"
            onKeyPress={handleKeyPress}
          />
        </Form>
      </ModalBody>
      <ModalFooter>
        <Button color="success" onClick={saveDeck}>
          Save
        </Button>
        <Button color="secondary" onClick={toggle}>
          Cancel
        </Button>
      </ModalFooter>
    </Modal>
  );
};

export default CreateDeckModal;
