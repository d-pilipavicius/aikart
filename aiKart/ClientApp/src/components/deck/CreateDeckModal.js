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
  isPublic,
  setIsPublic,
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
            onKeyDown={handleKeyPress}
          />
          <Input
            type="text"
            placeholder="Deck Description"
            value={newDeckDescription}
            onChange={(e) => setNewDeckDescription(e.target.value)}
            className="mt-2"
            onKeyDown={handleKeyPress}
          />
          <div className="mt-2">
            <Input
              type="checkbox"
              checked={isPublic}
              onChange={(e) => setIsPublic(e.target.checked)}
            />{" "}
            <span>Make this deck public</span>
          </div>
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
