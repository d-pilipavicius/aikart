import React, { useState } from "react";
import {
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Button,
  Form,
  FormGroup,
  Label,
  Input,
} from "reactstrap";
import { useDispatch, useSelector } from "react-redux";
import { generateDeck } from "../../app/state/deck/decksSlice";
import { fetchDecksByUser } from "../../app/state/user/userDecksSlice";

const GenerateDeckModal = ({ isOpen, toggle }) => {
  const dispatch = useDispatch();
  const user = useSelector((state) => state.users.currentUser);

  const [category, setCategory] = useState(9);
  const [numberOfCards, setNumberOfCards] = useState(1);

  const handleSubmit = () => {
    if (numberOfCards < 1 || numberOfCards > 50) {
      alert("Number of cards must be between 1 and 50.");
      return;
    }

    dispatch(
      generateDeck({
        CategoryId: category,
        NumberOfCards: numberOfCards,
        CreatorId: user.id,
      })
    ).then(() => {
      dispatch(fetchDecksByUser(user.id));
    });

    toggle();
  };

  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Choose Trivia Options</ModalHeader>
      <ModalBody>
        <Form>
          <FormGroup>
            <Label for="category">Category</Label>
            <Input
              type="select"
              name="category"
              id="category"
              value={category}
              onChange={(e) => setCategory(e.target.value)}
            >
              <option value="9">General Knowledge</option>
              <option value="10">Entertainment: Books</option>
              <option value="11">Entertainment: Film</option>
              <option value="12">Entertainment: Music</option>
              <option value="13">Entertainment: Musicals &amp; Theatres</option>
              <option value="14">Entertainment: Television</option>
              <option value="15">Entertainment: Video Games</option>
              <option value="16">Entertainment: Board Games</option>
              <option value="17">Science &amp; Nature</option>
              <option value="18">Science: Computers</option>
              <option value="19">Science: Mathematics</option>
              <option value="20">Mythology</option>
              <option value="21">Sports</option>
              <option value="22">Geography</option>
              <option value="23">History</option>
              <option value="24">Politics</option>
              <option value="25">Art</option>
              <option value="26">Celebrities</option>
              <option value="27">Animals</option>
              <option value="28">Vehicles</option>
              <option value="29">Entertainment: Comics</option>
              <option value="30">Science: Gadgets</option>
              <option value="31">
                Entertainment: Japanese Anime &amp; Manga
              </option>
              <option value="32">
                Entertainment: Cartoon &amp; Animations
              </option>
            </Input>
          </FormGroup>
          <FormGroup>
            <Label for="numberOfCards">Number of Cards</Label>
            <Input
              type="number"
              name="numberOfCards"
              id="numberOfCards"
              value={numberOfCards}
              onChange={(e) => setNumberOfCards(parseInt(e.target.value, 10))}
              min={1}
              max={50}
            />
          </FormGroup>
        </Form>
      </ModalBody>
      <ModalFooter>
        <Button color="primary" onClick={handleSubmit}>
          Generate Deck
        </Button>{" "}
        <Button color="secondary" onClick={toggle}>
          Cancel
        </Button>
      </ModalFooter>
    </Modal>
  );
};

export default GenerateDeckModal;
