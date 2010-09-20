// Copyright 2010, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Author: api.anash@gmail.com (Anash P. Oommen)

using com.google.api.adwords.lib;
using com.google.api.adwords.v201008;

using System;
using System.IO;
using System.Net;

namespace com.google.api.adwords.examples.v201008 {
  /// <summary>
  /// This code example retrieves urls that have content keywords related
  /// to a given website.
  ///
  /// Tags: TargetingIdeaService.get
  /// </summary>
  class GetRelatedPlacements : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example retrieves urls that have content keywords related to a" +
            " given website.";
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The AdWords user object running the code example.
    /// </param>
    public override void Run(AdWordsUser user) {
      // Get the TargetingIdeaService.
      TargetingIdeaService targetingIdeaService =
          (TargetingIdeaService) user.GetService(AdWordsService.v201008.TargetingIdeaService);

      string urlText = "mars.google.com";

      RelatedToUrlSearchParameter searchParameter = new RelatedToUrlSearchParameter();
      searchParameter.urls = new string[] {urlText};
      searchParameter.includeSubUrlsSpecified = true;
      searchParameter.includeSubUrls = false;

      TargetingIdeaSelector selector = new TargetingIdeaSelector();
      selector.searchParameters = new SearchParameter[] {searchParameter};
      selector.ideaTypeSpecified = true;
      selector.ideaType = IdeaType.PLACEMENT;
      selector.requestTypeSpecified = true;
      selector.requestType = RequestType.IDEAS;

      Paging paging = new Paging();
      paging.startIndex = 0;
      paging.startIndexSpecified = true;
      paging.numberResults = 10;
      paging.numberResultsSpecified = true;

      selector.paging = paging;

      try {
        TargetingIdeaPage page = targetingIdeaService.get(selector);

        if (page != null && page.entries != null) {
          Console.WriteLine("There are a total of {0} urls with content keywords related to" +
            " '{1}'. The first {2} entries are displayed below: \n", page.totalNumEntries, urlText,
            page.entries.Length);

          foreach (TargetingIdea idea in page.entries) {
            foreach (Type_AttributeMapEntry entry in idea.data) {
              if (entry.key == AttributeType.PLACEMENT) {
                PlacementAttribute placementAttribute = entry.value as PlacementAttribute;
                Console.WriteLine("Related content keywords were found at '{0}'.",
                    placementAttribute.value.url);
              }
            }
          }
        } else {
          Console.WriteLine("No urls with content keywords related to your url were found.");
        }
      } catch (Exception ex) {
        Console.WriteLine("Failed to retrieve related placements. Exception says \"{0}\"",
            ex.Message);
      }
    }
  }
}
