
$(document).ready(function () {
    $("#LeagueId").change(function () {
        $("#TeamId").empty();
        $.ajax({
            type: 'POST',
            url: Url,
            dataType: 'json',
            data: { leagueId: $("#LeagueId").val() },
            success: function (teams) {
                $.each(teams, function (i, team) {
                    $("#TeamId").append('<option value="'
                     + team.TeamId + '">'
                     + team.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve teams.' + ex);
            }
        });
        return false;
    });


    $("#LocalLeagueId").change(function () {
        $("#LocalId").empty();
        $.ajax({
            type: 'POST',
            url: Url,
            dataType: 'json',
            data: { leagueId: $("#LocalLeagueId").val() },
            success: function (teams) {
                $.each(teams, function (i, team) {
                    $("#LocalId").append('<option value="'
                     + team.TeamId + '">'
                     + team.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve teams.' + ex);
            }
        });
        return false;
    });


    $("#VisitorLeagueId").change(function () {
        $("#VisitorId").empty();
        $.ajax({
            type: 'POST',
            url: Url,
            dataType: 'json',
            data: { leagueId: $("#VisitorLeagueId").val() },
            success: function (teams) {
                $.each(teams, function (i, team) {
                    $("#VisitorId").append('<option value="'
                     + team.TeamId + '">'
                     + team.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve teams.' + ex);
            }
        });
        return false;
    });
});