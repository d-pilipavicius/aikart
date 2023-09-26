import React from "react";
import { useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";

const Home = () => {
  const decks = useSelector((state) => state.decks.decks);
  const navigate = useNavigate();

  return (
    <div>
      <h1>Welcome to AiKart!</h1>
      <h5>Select a deck you want to practice or create a new one by pressing <strong>Create Deck</strong>.</h5>
      <h5>Press <strong>Manage Decks</strong> to edit or delete existing decks.</h5>
      <br />

      <div className="row">
        {decks.map((deck, index) => (
          <div className="col-md-4" key={index}>
            <div
              className="card mb-4"
              onClick={() => navigate(`/practice/${deck.name}`)}
              style={{ cursor: "pointer" }}
            >
              <div className="card-body">
                <h5 className="card-title">
                  <Link
                    to={`/practice/${deck.name}`}
                    onClick={(e) => e.stopPropagation()}
                    className="text-dark"
                    style={{ textDecoration: "none" }}
                  >
                    {deck.name}
                  </Link>
                </h5>
                <p className="card-text">{deck.description}</p>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Home
