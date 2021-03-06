﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

// Various tests for:
// Microsoft.FSharp.Core.Result

namespace FSharp.Core.Unittests.FSharp_Core.Microsoft_FSharp_Core

open System
open FSharp.Core.Unittests.LibraryTestFx
open NUnit.Framework

type EmailValidation=
    | Empty
    | NoAt

module Results=
    let bind f m = match m with Error e -> Error e | Ok x -> f x

open Results

[<TestFixture>]
type ResultTests() =

    let fail_if_empty email=
        if String.IsNullOrEmpty(email) then Error Empty else Ok email

    let fail_if_not_at (email:string)=
        if (email.Contains("@")) then Ok email else Error NoAt

    let validate_email =
        fail_if_empty
        >> bind fail_if_not_at

    let test_validate_email email (expected:Result<string,EmailValidation>) =
        let actual = validate_email email
        Assert.AreEqual(expected, actual)


    [<Test>]
    member this.CanChainTogetherSuccessiveValidations() =
        test_validate_email "" (Error Empty)
        test_validate_email "something_else" (Error NoAt)
        test_validate_email "some@email.com" (Ok "some@email.com")

