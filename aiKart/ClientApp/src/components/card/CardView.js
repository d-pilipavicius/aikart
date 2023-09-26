import React, { useState, useEffect } from 'react'
import { Card, CardBody, CardTitle, CardText } from "reactstrap";

const CardView = (props) => {
  const [flip, setFlip] = useState(false)

  useEffect(() => {
    setFlip(false)
  }, [props.card])

  return (
    <Card className="mb-3" onClick={() => setFlip(!flip)}>
      <CardBody>
        <CardTitle tag="h5">{flip ? "Answer" : "Question"}</CardTitle>
        <CardText>{flip ? props.card.answer : props.card.question}</CardText>
      </CardBody>
    </Card>
  )
}

export default CardView