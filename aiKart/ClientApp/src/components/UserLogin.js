import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addUser, setCurrentUser } from "../app/state/user/usersSlice";

const UserLogin = () => {
  const [username, setUsername] = useState("");
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogin = async () => {
    const newUser = { name: username };

    // Dispatch the addUser action to add the user and set the currently logged-in user
    dispatch(addUser(newUser))
      .then((response) => {
        dispatch(setCurrentUser(response.payload));
        console.log(response.payload);
        navigate(`/home`);
      })
      .catch((error) => {
        console.error("User login failed", error);
      });
  };

  return (
    <div>
      <h1>Login</h1>
      <input
        type="text"
        placeholder="Enter your username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <button onClick={handleLogin}>Login</button>
    </div>
  );
};

export default UserLogin;
