import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import EditDeckModal from "./EditDeckModal";
import CreateDeckModal from "./CreateDeckModal";
import { deleteDeck, addDeck } from "../../app/state/deck/decksSlice";
import { fetchDecksByUser } from "../../app/state/user/userDecksSlice";
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
     dispatch(deleteDeck(deck.id)).then(
       () => {
         dispatch(fetchDecksByUser(user.id));
       }
     );
   }
 };


  const handleEdit = (event, deck) => {
    event.stopPropagation();
    setSelectedDeck(deck);
  };



  return (
    <div className="container">
      <h1 className="mb-4">Manage Decks</h1>

      <Button color="primary" className="mb-5" onClick={toggleCreateDeckForm}>
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

      <div className="row">
        {decks.map((deck, index) => (
          <div className="col-md-4" key={index}>
            <Card
              className="mb-4"
              onClick={() => navigate(`/decks/${deck.id}`)}
              style={{ cursor: "pointer" }}
            >
              <CardHeader className="bg-primary bg-gradient">
                <Link
                  to={`/decks/${deck.id}`}
                  onClick={(e) => e.stopPropagation()}
                  className="text-light"
                  style={{ textDecoration: "none" }}
                >
                  {deck.name}
                </Link>
              </CardHeader>
              <CardBody>
                <CardText>{deck.description}</CardText>
              </CardBody>
              <CardFooter>
                <Button
                  color="warning"
                  style={{ marginRight: "1rem" }}
                  onClick={(e) => handleEdit(e, deck)}
                >
                  Edit
                </Button>
                <Button color="danger" onClick={(e) => handleDelete(e, deck)}>
                  Delete
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
