import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import EditDeckModal from "./EditDeckModal";
import CreateDeckModal from "./CreateDeckModal";
import GenerateDeckModal from "./GenerateDeckModal";
import {
  deleteDeck,
  addDeck,
  resetRepetitionIntervalForDeck,
} from "../../app/state/deck/decksSlice";
import {
  fetchDecksByUser,
  fetchUserDeckStatistics,
} from "../../app/state/user/userDecksSlice";
import {
  Button,
  Card,
  CardText,
  CardBody,
  CardFooter,
  CardHeader,
} from "reactstrap";

const ManageDecks = () => {
  const decks = useSelector((state) => state.userDecks.userDecks);
  const user = useSelector((state) => state.users.currentUser);
  const [selectedDeck, setSelectedDeck] = useState(null);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [showCreateDeckForm, setShowCreateDeckForm] = useState(false);
  const [showGenerateDeckForm, setShowGenerateDeckForm] = useState(false);
  const [newDeckName, setNewDeckName] = useState("");
  const [newDeckDescription, setNewDeckDescription] = useState("");
  const [isPublic, setIsPublic] = useState(false);

  useEffect(() => {
    if (user) {
      dispatch(fetchDecksByUser(user.id));
    }
  }, [dispatch, user]);

  const toggleCreateDeckForm = () => {
    setShowCreateDeckForm(!showCreateDeckForm);
  };
  const toggleGenerateDeckForm = () => {
    setShowGenerateDeckForm(!showGenerateDeckForm);
  };

  const saveDeck = async (event) => {
    event.preventDefault();
    if (newDeckName && newDeckDescription) {
      await dispatch(
        addDeck({
          name: newDeckName,
          description: newDeckDescription,
          creatorId: user.id,
          creatorName: user.name,
          isPublic,
        })
      );
      setNewDeckName("");
      setNewDeckDescription("");
      setIsPublic(false);
      toggleCreateDeckForm();
      dispatch(fetchDecksByUser(user.id));
    }
  };

  const handleDelete = (event, deck) => {
    event.stopPropagation();
    if (window.confirm("Are you sure you want to delete this deck?")) {
      dispatch(deleteDeck(deck.id)).then(() => {
        dispatch(fetchDecksByUser(user.id));
      });
    }
  };

  const handleEdit = (event, deck) => {
    event.stopPropagation();
    setSelectedDeck(deck);
  };

  const formatDate = (timestamp) => {
    const date = new Date(timestamp);
    return date.toLocaleDateString(); // Format to display only the date
  };

  return (
    <div className="container">
      <h1 className="mb-4">Manage Decks</h1>

      <div className="mb-5">
        <Button
          color="primary"
          style={{ marginRight: "1rem" }}
          onClick={toggleCreateDeckForm}
        >
          Create Deck
        </Button>
        <CreateDeckModal
          isOpen={showCreateDeckForm}
          toggle={toggleCreateDeckForm}
          saveDeck={saveDeck}
          newDeckName={newDeckName}
          setNewDeckName={setNewDeckName}
          newDeckDescription={newDeckDescription}
          setNewDeckDescription={setNewDeckDescription}
          isPublic={isPublic}
          setIsPublic={setIsPublic}
        />

        <Button color="primary" onClick={toggleGenerateDeckForm}>
          Generate deck with Open TriviaDB
        </Button>
        <GenerateDeckModal
          isOpen={showGenerateDeckForm}
          toggle={toggleGenerateDeckForm}
        />
      </div>

      <div className="row">
        {decks.map((deck, index) => (
          <div className="col-md-6" key={index}>
            <Card className="mb-4">
              <CardHeader
                className={`bg-${
                  deck.isPublic ? "success" : "primary"
                } bg-gradient`}
                onClick={() => navigate(`/decks/${deck.id}`)}
                style={{ cursor: "pointer" }}
              >
                <Link
                  to={`/decks/${deck.id}`}
                  onClick={(e) => e.stopPropagation()}
                  className="text-light"
                  style={{ textDecoration: "none" }}
                >
                  {deck.name}
                </Link>
              </CardHeader>
              <CardBody
                onClick={() => navigate(`/decks/${deck.id}`)}
                style={{ cursor: "pointer" }}
              >
                <CardText>{deck.description}</CardText>
                <div className="row text-muted mb-1">
                  <div className="col-md-6">
                    <div>Number of cards: {deck.cards.length}</div>
                    <div>Author: {deck.creatorName}</div>
                    <div>Created: {formatDate(deck.creationDate)}</div>
                    <div>Last Edit: {formatDate(deck.lastEditDate)}</div>
                  </div>
                  <div className="col-md-6">
                    <div>Correct answers: {deck.statistics.correctAnswers}</div>
                    <div>
                      Incomplete answers:{" "}
                      {deck.statistics.partiallyCorrectAnswers}
                    </div>
                    <div>Wrong answers: {deck.statistics.incorrectAnswers}</div>
                    <div>Times solved: {deck.statistics.timesSolved}</div>
                  </div>
                </div>
              </CardBody>
              <CardFooter className="text-muted">
                <Button
                  color="warning"
                  style={{ marginRight: "1rem" }}
                  onClick={(e) => handleEdit(e, deck)}
                >
                  Edit
                </Button>
                <Button
                  color="danger"
                  style={{ marginRight: "1rem" }}
                  onClick={(e) => handleDelete(e, deck)}
                >
                  Delete
                </Button>
                <Button
                  color="secondary"
                  style={{ marginRight: "3rem" }}
                  onClick={() => navigate(`/practice/${deck.id}`)}
                >
                  Practice without tracking
                </Button>
                <Button
                  color="secondary"
                  onClick={() => () => dispatch(resetRepetitionIntervalForDeck(deck.id))}
                >
                  Reset stats
                </Button>
              </CardFooter>
            </Card>
          </div>
        ))}
      </div>

      {selectedDeck && (
        <EditDeckModal
          deck={selectedDeck}
          isOpen={Boolean(selectedDeck)}
          toggle={() => {
            setSelectedDeck(null);
          }}
          userId={user.id}
        />
      )}
    </div>
  );
};

export default ManageDecks;
