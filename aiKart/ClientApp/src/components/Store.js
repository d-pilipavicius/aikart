import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchDecks } from "../app/state/deck/decksSlice";
import {
  useTable,
  useSortBy,
  useGlobalFilter,
  usePagination,
} from "react-table";
import { Card, CardBody, Table, Button, Input } from "reactstrap";

const Store = () => {
  const dispatch = useDispatch();
  const decks = useSelector((state) => state.decks.decks);

  useEffect(() => {
    dispatch(fetchDecks());
  }, [dispatch]);

  const data = React.useMemo(() => decks, [decks]);
  const columns = React.useMemo(
    () => [
      {
        Header: "Deck Name",
        accessor: "name",
        width: "20%",
      },
      {
        Header: "Description",
        accessor: "description",
        width: "50%",
      },
      {
        Header: "Creator",
        accessor: "creatorname",
        width: "10%",
      },
      {
        Header: "Last edit date",
        accessor: "lasteditdate",
        width: "10%",
      },
      {
        Header: "Actions",
        accessor: "id",
        Cell: ({ value }) => (
          <Button color="success" onClick={() => handlePurchase(value)}>
            Purchase
          </Button>
        ),
        width: "10%",
      },
    ],
    []
  );

  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    page,
    prepareRow,
    state,
    setGlobalFilter,
    nextPage,
    previousPage,
    canNextPage,
    canPreviousPage,
    pageOptions,
    pageCount,
    gotoPage,
  } = useTable(
    { columns, data, initialState: { pageIndex: 0, pageSize: 10 } },
    useGlobalFilter,
    useSortBy,
    usePagination
  );

  const { globalFilter, pageIndex, } = state;

  const handleRefresh = () => {
    dispatch(fetchDecks());
  };

  const handlePurchase = (deckId) => {
    console.log(`Purchased deck with ID: ${deckId}`);
  };

  return (
    <div>
      <h1>Deck Store</h1>
      <Button color="primary" onClick={handleRefresh}>
        Refresh Decks
      </Button>
      <Input
        type="text"
        placeholder="Search decks..."
        value={globalFilter || ""}
        onChange={(e) => setGlobalFilter(e.target.value)}
        className="mt-3"
      />
      <Table striped hover {...getTableProps()} className="mt-3">
        <thead>
          {headerGroups.map((headerGroup) => (
            <tr {...headerGroup.getHeaderGroupProps()}>
              {headerGroup.headers.map((column) => (
                <th
                  {...column.getHeaderProps(column.getSortByToggleProps())}
                  style={{ width: column.width }}
                >
                  {column.render("Header")}
                  <span>
                    {column.isSorted
                      ? column.isSortedDesc
                        ? " 🔽"
                        : " 🔼"
                      : ""}
                  </span>
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody {...getTableBodyProps()}>
          {page.map((row) => {
            prepareRow(row);
            return (
              <tr {...row.getRowProps()}>
                {row.cells.map((cell) => (
                  <td
                    {...cell.getCellProps()}
                    style={{ width: cell.column.width }}
                  >
                    {cell.render("Cell")}{" "}
                  </td>
                ))}
              </tr>
            );
          })}
        </tbody>
      </Table>
      <div>
        <Button
          color="primary"
          onClick={() => gotoPage(0)}
          disabled={!canPreviousPage}
        >
          {"<<"}
        </Button>{" "}
        <Button
          color="primary"
          onClick={() => previousPage()}
          disabled={!canPreviousPage}
        >
          {"<"}
        </Button>{" "}
        <Button
          color="primary"
          onClick={() => nextPage()}
          disabled={!canNextPage}
        >
          {">"}
        </Button>{" "}
        <Button
          color="primary"
          onClick={() => gotoPage(pageCount - 1)}
          disabled={!canNextPage}
        >
          {">>"}
        </Button>{" "}
        <span>
          Page{" "}
          <strong>
            {pageIndex + 1} of {pageOptions.length}
          </strong>{" "}
        </span>
      </div>
    </div>
  );
};

export default Store;
