import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";

export const fetchUsers = createAsyncThunk("users/fetchUsers", async () => {
  const response = await axios.get("/api/user");
  return response.data;
});

export const fetchUserById = createAsyncThunk("users/fetchUserById", async (userId) => {
  const response = await axios.get(`/api/user/${userId}`);
  return response.data;
});

export const addUser = createAsyncThunk("users/addUser", async (userDto) => {
  const response = await axios.post("/api/user", userDto);
  return response.data;
});

const usersSlice = createSlice({
  name: "users",
  initialState: {
    users: [],
    currentUser: null,
    loading: "idle",
  },
  reducers: {
    setCurrentUser: (state, action) => {
      state.currentUser = action.payload;
    },
    logout: (state) => {
      state.currentUser = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchUsers.fulfilled, (state, action) => {
        state.loading = "idle";
        state.users = action.payload;
      })
      .addCase(fetchUsers.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(fetchUsers.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(fetchUserById.fulfilled, (state, action) => {
        state.loading = "idle";
        state.users.push(action.payload);
      })
      .addCase(fetchUserById.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(fetchUserById.rejected, (state, action) => {
        state.loading = "idle";
      })
      .addCase(addUser.fulfilled, (state, action) => {
        state.loading = "idle";
        state.users.push(action.payload);
        state.currentUser = action.payload;
      })
      .addCase(addUser.pending, (state, action) => {
        state.loading = "pending";
      })
      .addCase(addUser.rejected, (state, action) => {
        state.loading = "idle";
      });
  },
});

export const { setCurrentUser, logout } = usersSlice.actions;

export default usersSlice.reducer;
