import React, { useState } from "react"
import { useSelector } from "react-redux"
import { useParams, useNavigate} from "react-router-dom"
import CardView from "./card/CardView"
import { Button, Container } from "reactstrap"


const DeckPractice = () => {
    const { deckName } = useParams();
    const deck = useSelector((state) =>
        state.decks.decks.find((deck) => deck.name === deckName)
    );
    const [currentCard, setCurrentCard] = useState(0);
    const navigate = useNavigate();

    const currentCardObject = deck && deck.cards[currentCard];

    return (
        <div>
            {currentCardObject &&
                <div style={{ cursor: "pointer" }}>
                <CardView card={currentCardObject}/>
                </div>
            }
            <Container className="d-flex justify-content-center align-items-end">
                <p>Answered &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remaining</p>
            </Container>
            <Container className="d-flex justify-content-center align-items-end">
                <p>{currentCard} &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; {deck.cards.length - currentCard}</p>
            </Container>
            <Container className="d-flex justify-content-center align-items-end">
                {currentCard > 0 ? (<Button onClick={() => {
                        setCurrentCard(currentCard - 1)
                        }}>Prev</Button>) : null
                }
                {currentCard < deck.cards.length - 1 ? (<Button type="button" color="primary" onClick={() => {
                        setCurrentCard(currentCard + 1)
                        }}>Next</Button>) : null
                }
                {currentCard === deck.cards.length - 1 ? (<Button type="button" color="primary" onClick={() => navigate(`/`)}>Finish</Button>) : null
                }
            </Container>
        </div>
    )
}
  
export default DeckPractice;

