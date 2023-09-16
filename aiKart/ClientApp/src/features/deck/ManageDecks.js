import React from 'react';
import { useDispatch, useSelector } from 'react-redux';


const ManageDecks = () => {
    const decks = useSelector(state => state.decks.decks);

    return (
    <div className="container">
      <h1 className="mb-4">Manage Decks</h1>
      <div className="row">
        {decks.map((deck, index) => (
          <div className="col-md-4" key={index}>
            <div className="card mb-4">
              <div className="card-body">
                <h5 className="card-title">{deck.name}</h5>
                <p className="card-text">{deck.description}</p>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ManageDecks;

