import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { deleteDeck } from "../../app/state/deck/decksSlice";
import EditDeckModal from "./EditDeckModal";
import { fetchDecks } from "../../app/state/deck/decksSlice";

const ManageDecks = () => {
  const decks = useSelector((state) => state.decks.decks);
  const [selectedDeck, setSelectedDeck] = useState(null);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    dispatch(fetchDecks());
  }, [dispatch]);

  const handleDelete = (event, deck) => {
    event.stopPropagation();
    if (window.confirm("Are you sure you want to delete this deck?")) {
      dispatch(deleteDeck(deck.id)).then(() => {
        dispatch(fetchDecks());
      });
    }
  };

  const handleEdit = (event, deck) => {
    event.stopPropagation();
    setSelectedDeck(deck);
  };

  return (
    <div className="container">
      <h1 className="mb-4">Manage Decks</h1>
      <div className="row">
        {decks.map((deck, index) => (
          <div className="col-md-4" key={index}>
            <div
              className="card mb-4"
              onClick={() => navigate(`/decks/${deck.id}`)}
              style={{ cursor: "pointer" }}
            >
              <div className="card-body">
                <h5 className="card-title">
                  <Link
                    to={`/decks/${deck.name}`}
                    onClick={(e) => e.stopPropagation()}
                    className="text-dark"
                    style={{ textDecoration: "none" }}
                  >
                    {deck.name}
                  </Link>
                </h5>
                <p className="card-text">{deck.description}</p>

                <button
                  className="btn btn-danger me-2"
                  onClick={(e) => handleDelete(e, deck)}
                >
                  Delete
                </button>
                <button
                  className="btn btn-primary"
                  onClick={(e) => handleEdit(e, deck)}
                >
                  Edit
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>
      {selectedDeck && (
        <EditDeckModal
          deck={selectedDeck}
          isOpen={Boolean(selectedDeck)}
          toggle={() => {
            // Refetch the decks list after editing
            dispatch(fetchDecks());
            setSelectedDeck(null);
          }}
        />
      )}
    </div>
  );
};

export default ManageDecks;
