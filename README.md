# Lattice

A very simple Web Api for managing tasks, teams and boards, similar to Trello and
Jira. Using _.NET 7, ASP.NET Core and Entity Framework Core_.

This project was developed as an technical test for a job position in the course
of 5 days.

### Some notes about the source code and design decisions

This project does not use what is commonly referred to as _Repositories_. Since
their purpose is to abstract data persistence operations - and I do not intend
to use any abstraction other than _Entity Framework Core_ - creating a new
abstraction to accommodate more possible kinds of data persistence using
_Repositories_ is redundant.
